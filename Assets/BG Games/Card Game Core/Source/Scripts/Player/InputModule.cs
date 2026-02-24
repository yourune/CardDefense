using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Player
{
    public class InputModule:MonoBehaviour
    {
        [SerializeField] private LayerMask _inputReadersLayers;

        private ISupportInputReading _mouseDownObject;
        private ISupportInputReading _mouseEnterObject;

        private Camera _camera;
        private PlayerId _player;

        [Inject]
        private void Construct(PlayerId player)
        {
            _player = player;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            bool mouseDown = Input.GetMouseButtonDown(0);
            bool mouseUp = Input.GetMouseButtonUp(0);
            bool mouseHeld = Input.GetMouseButton(0);


            ISupportInputReading inputReader;

            if (RaycastMousePos(out inputReader))
            {
                if (_mouseEnterObject != inputReader)
                {
                    _mouseEnterObject?.MouseExit(Input.mousePosition, _player);

                    if (!mouseDown && !mouseUp && !mouseHeld)
                    {
                        inputReader.MouseEnter(Input.mousePosition, _player);
                        _mouseEnterObject = inputReader;
                    }
                    else
                    {
                        _mouseEnterObject = null;
                    }
                }
                else if (mouseDown || mouseUp)
                {
                    _mouseEnterObject?.MouseExit(Input.mousePosition, _player);
                    _mouseEnterObject = null;
                }


                if (mouseDown)
                {
                    _mouseDownObject = inputReader;
                    inputReader.MouseDown(Input.mousePosition, _player);
                }
                
                if (mouseUp && _mouseDownObject == inputReader)
                {
                    inputReader.MouseUp(Input.mousePosition, _player);
                    _mouseDownObject = null;
                }
            }
            else if (_mouseEnterObject != null)
            {
                _mouseEnterObject.MouseExit(Input.mousePosition, _player);
                _mouseEnterObject = null;
            }

            if (mouseHeld && _mouseDownObject != null)
            {
                _mouseDownObject.MouseDrag(Input.mousePosition, _player);
            }

            if (mouseUp && _mouseDownObject != null)
            {
                _mouseDownObject.MouseUp(Input.mousePosition,_player);
                _mouseDownObject = null;
            }
        }

        private bool RaycastMousePos(out ISupportInputReading inputReader)
        {
            Vector2 rayOrigin = _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Vector2 rayDirection = Vector2.zero;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection,Mathf.Infinity,_inputReadersLayers);

            if (hit.collider != null)
            {
                inputReader = hit.collider.GetComponent<ISupportInputReading>();

                return inputReader != null;
            }

            inputReader = default(ISupportInputReading);
            return false;
        }
    }
}
