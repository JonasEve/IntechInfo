using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class CraStream : Stream
    {
        readonly Stream _baseStream;
        readonly Crabouillage _craMode;
        readonly byte[] _byteSecret;
        long _position;
        readonly Random _random;

        public CraStream(Stream baseStream, string secretKey, Crabouillage craMode)
        {
            if (baseStream == null) throw new ArgumentNullException();

            if (String.IsNullOrEmpty(secretKey)) throw new ArgumentNullException();

            if (craMode == Crabouillage.Crabouille && !baseStream.CanWrite || craMode == Crabouillage.Decrabouille && !baseStream.CanRead) throw new ArgumentException();


            _baseStream = baseStream;
            _craMode = craMode;
            _byteSecret = Encoding.UTF7.GetBytes(secretKey);
            int start = 1;
            for(int i = 0; i < _byteSecret.Length; i++)
            {
                start <<= 1;
                start += _byteSecret[i];
            }
            _random = new Random(start);
        }

        public override bool CanRead
        {
            get { return _craMode == Crabouillage.Decrabouille; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return _craMode == Crabouillage.Crabouille; }
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead) throw new InvalidOperationException();

            int len = _baseStream.Read(buffer, offset, count);

            if (len > 0)
            {
                int idxSecret = (int)(_position % _byteSecret.Length);

                for (int i = offset; i < offset + len; i++)
                {
                    byte secret = _byteSecret[++idxSecret % _byteSecret.Length];
                    secret ^= (byte)(_random.Next());
                    buffer[i] = (byte)(buffer[i] ^ secret);
                }

                _baseStream.Write(buffer, offset, count);

                _position += len;
            }

            return len;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite) throw new InvalidOperationException();
            
            int idxSecret = (int)(_position % _byteSecret.Length);

            for(int i = offset; i < offset + count; i++)
            {
                byte secret = _byteSecret[++idxSecret % _byteSecret.Length];
                secret ^= (byte)(_random.Next());
                buffer[i] = (byte)(buffer[i] ^ secret);
            }

            _baseStream.Write(buffer, offset, count);
        }
    }
}
