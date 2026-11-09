namespace DeltaOps;

// Offline plan for the source-verified velocity-only Teleport overload.
// Angles are degrees; horizontalSpeed and verticalSpeed are Source units/second.
// No origin change, collision clearance, travel distance or DF speed is inferred.
public readonly record struct NativeRushImpulse(float X,float Y,float Z)
{
    public static bool TryCreate(float yawDegrees,float horizontalSpeed,float verticalSpeed,
        out NativeRushImpulse impulse)
    {
        impulse=default;
        if(!float.IsFinite(yawDegrees)||!float.IsFinite(horizontalSpeed)||
           horizontalSpeed<=0||horizontalSpeed>1000||!float.IsFinite(verticalSpeed))return false;
        var radians=(double)yawDegrees*Math.PI/180;
        impulse=new((float)(Math.Cos(radians)*horizontalSpeed),
            (float)(Math.Sin(radians)*horizontalSpeed),verticalSpeed);
        return true;
    }
}
