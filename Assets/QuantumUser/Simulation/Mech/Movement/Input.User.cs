using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial struct Input
    {
        private const int FractionalBits = 16;  // 소수점 이하 비트 수
        private const int ScaleFactor = 1 << FractionalBits;
        
        public FPVector2 Movement
        {
            get => DecodeDirection(MovementEncoded);
            set => MovementEncoded = EncodeDirection(value);
        }
        public FPVector2 MousePosition
        {
            get
            {
                return new FPVector2(ScreenPositionX, ScreenPositionY);
            }
            set
            {
                ScreenPositionX = value.X;
                ScreenPositionY = value.Y;
            }
        } 
        
        private byte EncodeDirection(FPVector2 direction)
        {
            if (direction == default)
            {
                return default;
            }

            FP angle = FPVector2.RadiansSigned(FPVector2.Up, direction) * FP.Rad2Deg;
            angle = (((angle + 360) % 360) / 2) + 1;
            return (byte)angle.AsInt;
        }

        private FPVector2 DecodeDirection(int directionEncoded)
        {
            if (directionEncoded == default)
            {
                return default;
            }

            int angle = (directionEncoded - 1) * 2;
            return FPVector2.Rotate(FPVector2.Up, angle * FP.Deg2Rad);
        }
        
        public int Encode(FPVector2 vector)
        {
            // X와 Y 값을 고정 소수점 정수로 변환합니다.
            int x = (int)(vector.X * ScaleFactor);
            int y = (int)(vector.Y * ScaleFactor);
        
            // X 값을 상위 16비트, Y 값을 하위 16비트로 이동하여 결합합니다.
            return (x << 16) | (y & 0xFFFF);
        }

        public FPVector2 Decode(int encodedValue)
        {
            // 상위 16비트를 추출하여 X 값으로 사용
            int x = (encodedValue >> 16);
        
            // 하위 16비트를 추출하여 Y 값으로 사용
            int y = (encodedValue & 0xFFFF);

            // 고정 소수점 정수를 float로 변환합니다.
            return new FPVector2(x / FP.FromFloat_UNSAFE(ScaleFactor), y / FP.FromFloat_UNSAFE(ScaleFactor));
        }
    }
}