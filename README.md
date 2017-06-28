# geckoboard-c-sharp

C# client library for Geckoboard

## Installation

To be confirmed

## Usage

### Add a reference in your project

To be confirmed

### Ping to authenticate

Verify that your API key is valid and that you can reach the Geckoboard API.

```c#
var geckoboardClient = new Client("222efc82e7933138077b1c2554439e15");
geckoboardClient.Ping() // returns true
```

### Find or create

Verify an existing dataset or create a new one.

```c#
var Dataset = geckoboardClient.Datasets().FindOrCreate(
  "sales.gross", // Dataset ID
  new List<Field>(new Field[] { // Dataset Schema
    new MoneyField("cost", "Cost", false, "USD"),
    new DateTimeField("timestamp", "Time"),
    new NumberField("amount", "Amount", true)
  },
  "timestamp" // Unique By
);
```

`uniqueBy` is an optional array of one or more field names whose values will be unique across all your records.
