# ECS-Study
ECS是一種使用DOTS架構的開發框架，其設計邏輯有別於傳統OOP面向對象編程，反而更接近GPU批處理加速概念，DOTS我們一般稱為面向數據編程。

以下是四種Unity中不同框架下改善性能的方案

1、Classic System（傳統系統）：
這是傳統的Unity開發方式，使用MonoBehaviour組件和GameObject進行開發。這種方式在小型項目中很常見，但在大型項目中可能會遇到性能和可擴展性的問題。

2、Classic System + Job System（傳統系統 + Job系統）：
Unity的Job系統（Job System）是一種多線程執行任務的框架，可以提高遊戲的性能。你可以在傳統系統中使用Job System來執行並行任務，從而提高遊戲的效能。

3、ECS + Job System（實體組件系統 + 作業系統）：
實體組件系統（ECS）是Unity的一個新的開發模型，將遊戲中的實體（Entity）拆分為組件（Component）和系統（System）。ECS專注於數據導向的設計，可以更好地利用現代硬件的多核心處理能力。與Job System結合使用，可以獲得極高的性能。

4、ECS + Job System + Burst Compiler（實體組件系統 + 作業系統 + Burst編譯器）：
Burst編譯器是Unity的一個優化工具，可以將C#代碼轉換為高效的本機代碼。與ECS和Job System結合使用，Burst編譯器可以進一步提高遊戲的性能。

本Study案例會分別由這四種實作來測試各自的優缺點，以及探討適合使用的時機。
