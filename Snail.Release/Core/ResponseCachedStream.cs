using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 输出缓存流
    /// </summary>
    public class ResponseCachedStream : Stream
    {
        private readonly Stream _innerStream;
        private MemoryStream _cachedStream;
        private bool _disposed;

        internal ResponseCachedStream(Stream innerStream)
        {
            _innerStream = innerStream;
            _cachedStream = new MemoryStream();
        }

        public override bool CanRead => _innerStream.CanRead;

        public override bool CanSeek => _innerStream.CanSeek;

        public override bool CanWrite => _innerStream.CanWrite;

        public override long Length => _innerStream.Length;

        public override long Position
        {
            get { return _innerStream.Position; }
            set
            {
                DisableBuffering();
                _innerStream.Position = value;
            }
        }

        internal void DisableBuffering()
        {
            _cachedStream.Dispose();
        }

        public override void SetLength(long value)
        {
            DisableBuffering();
            _innerStream.SetLength(value);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            DisableBuffering();
            return _innerStream.Seek(offset, origin);
        }

        public override void Flush()
        {
            try
            {
                _innerStream.Flush();
            }
            catch
            {
                DisableBuffering();
                throw;
            }
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _innerStream.FlushAsync();
            }
            catch
            {
                DisableBuffering();
                throw;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
            => _innerStream.Read(buffer, offset, count);

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                _innerStream.Write(buffer, offset, count);
            }
            catch
            {
                DisableBuffering();
                throw;
            }
            this._cachedStream.Write(buffer, offset, count);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            try
            {
                await _innerStream.WriteAsync(buffer, offset, count, cancellationToken);
            }
            catch
            {
                DisableBuffering();
                throw;
            }
            await this._cachedStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            try
            {
                _innerStream.WriteByte(value);
            }
            catch
            {
                DisableBuffering();
                throw;
            }
            _cachedStream.WriteByte(value);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return ToIAsyncResult(WriteAsync(buffer, offset, count), callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException(nameof(asyncResult));
            }
            ((Task)asyncResult).GetAwaiter().GetResult();
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            _cachedStream.Position = 0;
            return _cachedStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }               

        internal byte[] ToArray()
        {
            _cachedStream.Position = 0;
            return _cachedStream.ToArray();
        }

        internal IAsyncResult ToIAsyncResult(Task task, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<int>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(0);
                }

                callback?.Invoke(tcs.Task);
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
            return tcs.Task;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_disposed)
                {
                    return;
                }
                if (disposing)
                {
                    _innerStream.Dispose();
                    DisableBuffering();
                }
                _disposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}

