using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class TeeStream : Stream
    {
        readonly Stream _out1;
        readonly Stream _out2;
        long _position;
        long _originPosition1;
        long _originPosition2;

        public TeeStream(Stream out1, Stream out2)
        {
            if (out1 == null || out2 == null) throw new ArgumentNullException();
            if (!out1.CanWrite || !out2.CanWrite) throw new ArgumentException("Impossible to write");

            _out1 = out1;
            _out2 = out2;

            if (_out1.CanSeek && _out2.CanSeek)
            {
                _originPosition1 = _out1.Position;
                _originPosition2 = _out2.Position;
            }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return _out1.CanSeek && _out2.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _out1.Flush();
            _out2.Flush();
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
                _out1.Position = _originPosition1 + value;
                _out2.Position = _originPosition2 + value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch(origin)
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
            _position += count;
            _out1.Write(buffer, offset, count);
            _out2.Write(buffer, offset, count);
        }
    }
}
