using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;
using Niantic.ARDK.AR.Awareness.Semantics;
using Niantic.ARDK.VirtualStudio.AR.Mock;


public class ArSegmentationManager : MonoBehaviour
{
    [SerializeField] private ARSemanticSegmentationManager segmentationManager;
    [SerializeField] private RawImage rawImageTexture;
    [SerializeField] private Shader segmentationShader;

    [Serializable]
    public struct ARSemanticSegmentation
    {
        public MockSemanticLabel.ChannelName ChannelType; // Serializable
        public Texture2D Texture;
    }

    public ARSemanticSegmentation arSemSeg;
    private Texture2D Mask;  // Used below

    // Start is called before the first frame update
    void Start()
    {
        rawImageTexture.material = new Material(segmentationShader);
        segmentationManager.SemanticBufferInitialized += SemanticBufferInitialized;
        segmentationManager.SemanticBufferUpdated += SemanticBufferUpdated;
    }

    private void OnDisable()
    {
        segmentationManager.SemanticBufferInitialized -= SemanticBufferInitialized;
        segmentationManager.SemanticBufferUpdated -= SemanticBufferUpdated;
    }

    private void SemanticBufferInitialized(ContextAwarenessArgs<ISemanticBuffer> args)
    {
        rawImageTexture.gameObject.SetActive(true);
    }
    private void SemanticBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        string channelName = arSemSeg.ChannelType.ToString().ToLower(); // Person, Ground, Sky...
        ISemanticBuffer semanticBuffer = args.Sender.AwarenessBuffer;
        int channelIndex = args.Sender.AwarenessBuffer.GetChannelIndex(channelName);

        semanticBuffer.CreateOrUpdateTextureARGB32(ref Mask, channelIndex);
        rawImageTexture.material.SetTexture("_SemanticTex", Mask);   // 가려야하는(Masking해야하는) 부분을 SemanticTex. 쉐이더에서 색변환
        rawImageTexture.material.SetMatrix("_semanticTransform", segmentationManager.SemanticBufferProcessor.SamplerTransform);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
