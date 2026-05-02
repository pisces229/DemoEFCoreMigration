# Model

PostgreSQL 命名規範指南

本文件定義了資料庫物件（View, Function, Procedure 等）的命名前綴規範，旨在提升程式碼可讀性、便於 IDE 排序，並減少開發過程中的名稱衝突。

## 1. 核心物件命名前綴

| 物件類型 | 前綴 | 範例 | 說明 |
| --- | --- | --- | --- |
| **View** | `view_` | `view_name` | 區分虛擬表與實體表。 |
| **Function** | `func_` | `func_name` | 用於 `SELECT` 運算或回傳資料。 |
| **Procedure** | `proc_` | `proc_name` | 用於 `CALL` 執行交易性操作。 |
| **Trigger** | `trig_` | `trig_name` | 附加在資料表上的觸發器。 |
| **Sequence** | `sequ_` | `sequ_name` | 序列產生器。 |

---

## 2. 函數與過程參數/變數規範 (Internal Level)

在撰寫 `PL/pgSQL` 時，強烈建議對參數與內部變數使用前綴，以避免與 SQL 語句中的資料表欄位名稱產生混淆。

### 參數 (Arguments)

* **輸入參數 (Input):** `i_` 
* 範例：`func/proc(i_name INT)`


* **輸出參數 (Output):** `o_`
* 範例：`func/proc(o_name OUT INT)`


* **輸入輸出參數 (In/Out):** `io_`
* 範例：`func/proc(io_name INOUT TEXT)`



### 內部變數 (Local Variables)

* **一般變數:** `v_` (Variable)
* 範例：`DECLARE v_name DECIMAL;`


* **常數:** `c_` (Constant)
* 範例：`DECLARE c_name CONSTANT REAL := 0.05;`


* **紀錄/迴圈變數:** `r_` (Record)
* 範例：`FOR r_name IN SELECT ...`

---

## 3. 命名重點整理

### 🔹 命名慣例 (Best Practices)

* **全小寫：** PostgreSQL 對大小寫不敏感（除非加雙引號），建議統一使用小寫。
* **蛇形命名 (Snake Case)：** 使用底線 `_` 分隔單字，例如 `get_order_count`。

---
