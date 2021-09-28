StrixActionGameSample
=====================

このプロジェクトはStrixCloudを使ったオンラインアクションゲームのサンプルです。
StrixUnitySDKのコンポーネントやRPCを使って同期処理を実現しています。

動作を確認するには、メインシーンであるAssets/Scenes/SampleSceneを開いてください。

次に通信の接続先サーバーの設定を行います。
StrixCloudのWebコンソール画面からサーバーを作成しておきます。

ヒエラルキーウィンドウからStrixConnectUI > StrixConnectPanelを選択しますと
インスペクターウィンドウにStrix Connect GUIというコンポーネントがあります。
このHostとApplicationIdに、Webコンソールで作成したサーバーのホストアドレスとアプリケーションIDをセットしてください。

これで、プレイしてサーバーに接続できるようになります。
マルチプレイヤーでの同期を確認したい場合は、プロジェクトをビルドしたものを複数プロセス起動して確認してください。


より詳しい説明はStrixCloudのドキュメンテーションにも書いてありますのでこちらもご参照ください。

https://www.strixcloud.net/private/docs/manual/ja/unity/html/servers.html
https://www.strixcloud.net/private/docs/manual/ja/unity/html/unitysdk/connect.html

