using System; 
public interface IMiniGame
{
    event Action MiniGameCompleted; 
    event Action MiniGameFailed;
    void StartMiniGame();
    void StopMiniGame();
    void FailMiniGame();
}
