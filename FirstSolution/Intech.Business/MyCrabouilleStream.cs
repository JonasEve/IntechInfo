using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public enum Crabouillage
    {
        Crabouille,
        Decrabouille
    }
    public class MyCrabouilleStream : Stream
    {
        readonly Stream _baseStream;
        Crabouillage _craMode;
        byte[] _byteSecret;
        long _position;
        long _originPositionBase;

        public MyCrabouilleStream(Stream baseStream, string secretKey, Crabouillage craMode)
        {
            if (baseStream == null) throw new ArgumentNullException();

            if (craMode == Crabouillage.Crabouille && !baseStream.CanWrite) throw new ArgumentException();

            if (craMode == Crabouillage.Decrabouille && !baseStream.CanRead) throw new ArgumentException();

            _baseStream = baseStream;
            _craMode = craMode;
            _byteSecret = GetBytes(secretKey);

            if (_baseStream.CanSeek)
            {
                _originPositionBase = _baseStream.Position;
            }
        }

        private byte[] GetBytes(string secretKey)
        {
            byte[] bytes = new byte[secretKey.Length * sizeof(char)];
            System.Buffer.BlockCopy(secretKey.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public override bool CanRead
        {
            get { return _craMode == Crabouillage.Decrabouille; }
        }

        public override bool CanSeek
        {
            get { return _baseStream.CanSeek; }
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
                if (!CanSeek) throw new NotSupportedException();
                _position = value;
                _baseStream.Position = _originPositionBase + value; ;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead) throw new InvalidOperationException();

            _position += count;

            int nb = _baseStream.Read(buffer, offset, count);

            for (int i = offset; i < offset + count; i++)
            {
                byte b = 0;
                for (int j = 0; j < _byteSecret.Length; j++)
                    b = (byte)(b + _byteSecret[j]);

                buffer[i] = (byte)(buffer[i] - b);
            }

            return nb;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: Position = offset; break;
                case SeekOrigin.End: throw new NotSupportedException();
                case SeekOrigin.Current: Position = _position + offset; break;
            }

            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite) throw new InvalidOperationException();

            _position += count;

            for (int i = offset; i < offset + count; i++)
            {
                byte b = 0;
                for (int j = 0; j < _byteSecret.Length; j++)
                    b = (byte)(b + _byteSecret[j]);

                buffer[i] = (byte)(buffer[i] + b);
            }

            _baseStream.Write(buffer, offset, count);
        }
    }
}
