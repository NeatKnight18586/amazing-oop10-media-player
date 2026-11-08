namespace mediaPlayerPrj;

/// <summary>Contract for common playback commands in the instructor's specification.</summary>
public interface IMediaControls
{
    void Play();
    void Pause();
    void Stop();
    void Next();
    void Previous();
}
