using System;

namespace ViToolkit.PSMoveSharp
{
    static class QuatHelper
    {
        public static Float4 QuatToEulerRad(Float4 quat)
        {
            Float4 euler;

            euler.y = Convert.ToSingle(Math.Asin(2.0 * ((quat.x * quat.z) - (quat.w * quat.y))));

            if (euler.y == 90.0)
            {
                euler.x = Convert.ToSingle(2.0 * Math.Atan2(quat.x, quat.w));
                euler.z = 0;
            }
            else if (euler.y == -90.0)
            {
                euler.x = Convert.ToSingle(-2.0 * Math.Atan2(quat.x, quat.w));
                euler.z = 0;
            }
            else
            {
                euler.x = Convert.ToSingle(Math.Atan2(2.0 * ((quat.x * quat.y) + (quat.z * quat.w)), 1.0 - (2.0 * ((quat.y * quat.y) + (quat.z * quat.z)))));
                euler.z = Convert.ToSingle(Math.Atan2(2.0 * ((quat.x * quat.w) + (quat.y * quat.z)), 1.0 - (2.0 * ((quat.z * quat.z) + (quat.w * quat.w)))));
            }

            euler.w = 0;

            return euler;
        }

        public static Float4 QuatToEulerDeg(Float4 quat)
        {
            return RadToDeg(QuatToEulerRad(quat));
        }

        public static Float4 RadToDeg(Float4 EulerQuat)
        {
            EulerQuat.x = Convert.ToInt32((180.0 / Math.PI) * EulerQuat.x);
            EulerQuat.y = Convert.ToInt32((180.0 / Math.PI) * EulerQuat.y);
            EulerQuat.z = Convert.ToInt32((180.0 / Math.PI) * EulerQuat.z);

            return EulerQuat;
        }
    }
}
