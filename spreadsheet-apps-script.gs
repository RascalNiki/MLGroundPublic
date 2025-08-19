// シート参照
const sheetTotal = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("通算成績");
const sheetDaily = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("デイリー")

// GitHub設定
const REPO_OWNER = "RascalNiki";
const REPO_NAME = "rascalniki.github.io";
const GITHUB_TOKEN = PropertiesService.getScriptProperties().getProperty("GITHUB_TOKEN");

function autoUpdateDate() {
  const now = new Date();
  sheetTotal.getRange("B2").setValue(now);
  sheetDaily.getRange("B2").setValue(now);
}

function createTableChart(sheet, headerRange, dataRange, width, height, fileName) {
  const header = sheet.getRange(headerRange).getDisplayValues();
  const values = sheet.getRange(dataRange).getDisplayValues();

  const table = Charts.newDataTable();
  header[0].forEach(col => table.addColumn(Charts.ColumnType.STRING, col));
  values.forEach(row => table.addRow(row));

  return Charts.newTableChart()
    .setDataTable(table.build())
    .setDimensions(width, height)
    .build()
    .getBlob()
    .setName(fileName);
}

// 通算成績用
function imageRangeTotal() {
  const blob = createTableChart(sheetTotal, "A2:G2", "A3:G12", 512, 512, "leaderboard.png");
  uploadToGitHub(blob, "leaderboard.png");
}

// デイリー用
function imageRangeDaily() {
  const blob = createTableChart(sheetDaily, "A2:C2", "A3:C5", 256, 128, "leaderboardDaily.png");
  uploadToGitHub(blob, "leaderboardDaily.png");
}

function uploadToGitHub(blob, filePath) {
  const url = `https://api.github.com/repos/${REPO_OWNER}/${REPO_NAME}/contents/${filePath}`;

  const payload = {
    message: "画像の自動アップロード",
    content: Utilities.base64Encode(blob.getBytes()),
    sha: getFileSHA(url, GITHUB_TOKEN) // 既存なら上書き
  };

  const options = {
    method: "put",
    contentType: "application/json",
    headers: { "Authorization": "token " + GITHUB_TOKEN },
    payload: JSON.stringify(payload)
  };

  try {
    const response = UrlFetchApp.fetch(url, options);
    Logger.log("アップロード成功：" + response.getContentText());
  } catch (e) {
    Logger.log("アップロード失敗：" + e.toString());
  }
}

function getFileSHA(url, token) {
  try {
    const response = UrlFetchApp.fetch(url, {
      method: "get",
      headers: { "Authorization": "token " + token }
    });
    return JSON.parse(response.getContentText()).sha || null;
  } catch (e) {
    Logger.log("SHA取得失敗（新規ファイルの場合は無視）：" + e.toString());
    return null;
  }
}
