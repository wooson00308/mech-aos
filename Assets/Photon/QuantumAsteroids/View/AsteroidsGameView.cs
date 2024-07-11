namespace Quantum.Asteroids
{
  using Quantum;
#if QUANTUM_ENABLE_TEXTMESHPRO
  using Text = TMPro.TextMeshProUGUI;
#else 
  using Text = UnityEngine.UI.Text;
#endif

  /// <summary>
  /// The AsteroidsGameView class is responsible for updating the game's UI elements
  /// such as the level text and scoreboard.
  /// </summary>
  public unsafe class AsteroidsGameView : QuantumSceneViewComponent
  {
    /// <summary>
    /// The UI Text element that displays the current game level.
    /// </summary>
    public Text LevelText;
    
    /// <summary>
    /// The UI Text element that displays the scoreboard with players' scores.
    /// </summary>
    public Text ScoreBoard;

    /// <summary>
    /// Updates the game view, including the level text and scoreboard.
    /// </summary>
    public override void OnUpdateView()
    {
      if (LevelText != null)
      {
        LevelText.text = $"Level {VerifiedFrame.Global->AsteroidsWaveCount}";
      }

      if (ScoreBoard != null)
      {
        ScoreBoard.text = "<b>Score</b>\n";
        var shipsFilter = VerifiedFrame.Filter<AsteroidsPlayerLink, AsteroidsShip>();
        while (shipsFilter.Next(out var entity, out var playerLink, out var shipFields))
        {
          var playerName = VerifiedFrame.GetPlayerData(playerLink.PlayerRef).PlayerNickname;
          ScoreBoard.text += $"{playerName}: {shipFields.Score}  \n";
        }
      }
    }
  }
}