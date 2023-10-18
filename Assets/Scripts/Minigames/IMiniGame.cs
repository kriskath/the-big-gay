using System; 
public interface IMiniGame
{
    event Action MiniGameCompleted; 
    void StartMiniGame();
    void StopMiniGame();
}
