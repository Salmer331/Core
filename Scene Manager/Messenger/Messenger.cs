using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.SceneManagement
{
    public class Messenger
    {
        public class MessengerSystem<T> : IDisposable
        {
            private CancellationTokenSource _ctx = new CancellationTokenSource();
            private bool _isMessageAwaited = false;
            private T _message;
            
            public void SendMessage(T answer)
            {
                _message = answer;
                _isMessageAwaited = false;
            }
        
            public async UniTask<T> WaitForMessage(CancellationToken token)
            {
                _ctx ??= new CancellationTokenSource();

                using var ctx = CancellationTokenSource.CreateLinkedTokenSource(_ctx.Token, token);
                try
                {
                    if (token.IsCancellationRequested)
                    {
                        Dispose();
                        return default(T);
                    }
                    
                    Debug.Log($"[Messenger] await\n{this}");
                    _isMessageAwaited = true;
                    await UniTask.WaitWhile(() => _isMessageAwaited, PlayerLoopTiming.Update, ctx.Token);
                    Debug.Log($"[Messenger] received\n{this}");
                    _isMessageAwaited = false;
                    return _message;
                }
                catch (Exception e)
                {
                    Dispose();
                    if (e.IsOperationCanceledException())
                    {
                        Debug.Log($"[Messenger] canceled\n{this}");
                        return default(T);
                    }
                    throw;
                }
            }
            
            public void Dispose()
            {
                Debug.Log($"[Messenger] dispose\n{this}");
                _isMessageAwaited = false;
                _ctx?.Cancel();
                _ctx?.Dispose();
                _ctx = null;
            }
        }
    }
}