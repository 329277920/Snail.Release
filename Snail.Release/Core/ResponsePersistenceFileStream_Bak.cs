using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 支持持久化的输出流
    /// </summary>
    public class ResponsePersistenceFileStream_Bak : FileStream
    {
        private readonly Stream _innerStream;        

        public ResponsePersistenceFileStream_Bak(Stream innerStream, string filePath)
            : base(filePath, FileMode.Create, FileAccess.Write, FileShare.Read)
        {
            _innerStream = innerStream;                
        }                                            

        public override void Flush()
        {
            try
            {
                _innerStream.Flush();
            }            
            finally
            {
                base.Flush();
            }
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _innerStream.FlushAsync(cancellationToken);
            }
            finally
            {
                await base.FlushAsync(cancellationToken);
            }
        }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                _innerStream.Write(buffer, offset, count);
            }
            finally
            {
                base.Write(buffer, offset, count);
            }             
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            try
            {
                await _innerStream.WriteAsync(buffer, offset, count, cancellationToken);
            }
            finally
            {
                await base.WriteAsync(buffer, offset, count, cancellationToken);
            }             
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this._innerStream.Dispose();
            }
        }

        //public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        //{
        //    var task = WriteAsync(buffer, offset, count, default(CancellationToken), state);
        //    if (callback != null)
        //    {
        //        task.ContinueWith(t => callback.Invoke(t));
        //    }
        //    return task;
        //}

        //private Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, object state)
        //{
        //    var tcs = new TaskCompletionSource<object>(state);
        //    var task = WriteAsync(buffer, offset, count, cancellationToken);
        //    task.ContinueWith((task2, state2) =>
        //    {
        //        var tcs2 = (TaskCompletionSource<object>)state2;
        //        if (task2.IsCanceled)
        //        {
        //            tcs2.SetCanceled();
        //        }
        //        else if (task2.IsFaulted)
        //        {
        //            tcs2.SetException(task2.Exception);
        //        }
        //        else
        //        {
        //            tcs2.SetResult(null);
        //        }
        //    }, tcs, cancellationToken);
        //    return tcs.Task;
        //}


    }
}

