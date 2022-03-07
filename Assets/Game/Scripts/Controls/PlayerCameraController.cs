using Assets.Game.Scripts.Monopoly.Enums;
using Assets.ItemInspection.Scripts.Utils;
using Cinemachine;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Controls
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] public CinemachineFreeLook virtualCamera = null;

        [SerializeField] public Transform figure;
        [SerializeField] public Transform center;

        CameraFollowType CameraView
            => (CameraFollowType)cameraViewInd;
        int cameraViewInd = 1;

        public override void OnStartAuthority()
        {
            virtualCamera.gameObject.SetActive(true);          

            enabled = true;
        }

        private void ChangeCameraView()
        {
            cameraViewInd++;
            cameraViewInd %= 4;

            BuildCameraView();
        }

        private void BuildCameraView()
        {
            switch (CameraView)
            {
                case CameraFollowType.FollowPlayerLookPlayer:
                    virtualCamera.Follow = figure.transform;
                    virtualCamera.LookAt = figure.transform;

                    virtualCamera.m_Orbits[0].m_Radius = 20f;
                    virtualCamera.m_Orbits[1].m_Radius = 15f;
                    virtualCamera.m_Orbits[2].m_Radius = 10f;

                    virtualCamera.m_Orbits[0].m_Height = 20f;
                    virtualCamera.m_Orbits[1].m_Height = 15f;
                    virtualCamera.m_Orbits[2].m_Height = 1f;
                    break;
                case CameraFollowType.FollowPlayerLookCenter:
                    virtualCamera.Follow = figure.transform;
                    virtualCamera.LookAt = center.transform;

                    virtualCamera.m_Orbits[0].m_Radius = 20f;
                    virtualCamera.m_Orbits[1].m_Radius = 15f;
                    virtualCamera.m_Orbits[2].m_Radius = 10f;

                    virtualCamera.m_Orbits[0].m_Height = 20f;
                    virtualCamera.m_Orbits[1].m_Height = 15f;
                    virtualCamera.m_Orbits[2].m_Height = 1f;
                    break;
                case CameraFollowType.FollowCenterLookCenter:
                    virtualCamera.Follow = center.transform;
                    virtualCamera.LookAt = center.transform;

                    virtualCamera.m_Orbits[0].m_Radius = 50f;
                    virtualCamera.m_Orbits[1].m_Radius = 40f;
                    virtualCamera.m_Orbits[2].m_Radius = 30f;

                    virtualCamera.m_Orbits[0].m_Height = 50f;
                    virtualCamera.m_Orbits[1].m_Height = 30f;
                    virtualCamera.m_Orbits[2].m_Height = 1f;
                    break;
                case CameraFollowType.FollowCenterLookPlayer:
                    virtualCamera.Follow = center.transform;
                    virtualCamera.LookAt = figure.transform;

                    virtualCamera.m_Orbits[0].m_Radius = 20f;
                    virtualCamera.m_Orbits[1].m_Radius = 15f;
                    virtualCamera.m_Orbits[2].m_Radius = 10f;

                    virtualCamera.m_Orbits[0].m_Height = 20f;
                    virtualCamera.m_Orbits[1].m_Height = 15f;
                    virtualCamera.m_Orbits[2].m_Height = 1f;
                    break;
                default:
                    break;
            }
        }

        [ClientCallback]
        private void OnEnable()
        { 
            virtualCamera.enabled = true;
        }

        [ClientCallback]
        private void OnDisable()
        {
            virtualCamera.enabled = false;
        }

        private void Update()
        {
            if (!hasAuthority)
                return;

            if (Input.GetKeyDown(KeyCode.Q))
                ChangeCameraView();
        }
    }
}
