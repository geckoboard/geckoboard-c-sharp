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
var dataset = geckoboardClient.Datasets().FindOrCreate(
  "sales.gross", // Dataset ID
  new List<Field>(new Field[] { // Dataset Schema
    new MoneyField("cost", "Cost", true, "USD"),
    new DateTimeField("timestamp", "Time"),
    new NumberField("amount", "Amount", true)
  },
  "timestamp" // Unique By
);
```

Available field types:

- `DateField`
- `DateTimeField`
- `NumberField`
- `PercentageField`
- `StringField`
- `MoneyField`

`uniqueBy` is an optional array of one or more field names whose values will be unique across all your records.

### Delete

Delete a dataset with a given id.

```c#
geckoboardClient.Datasets().Delete("sales.gross"); // returns true
```

### Put

Replace all data in the dataset.

```c#
dataset.Put(new Dictionary<string, object>[] {
  new Dictionary<string, object> { 
    { "timestamp", new DateTime(2016, 1, 2, 12, 0, 0) },
    { "amount", 40900 }
  },
  new Dictionary<string, object> {
    { "timestamp", new DateTime(2016, 1, 3, 12, 0, 0) },
    { "amount", 16400 }
  }
});
```

### Post

Append data to a dataset.

```c#
dataset.Put(
  new Dictionary<string, object>[] {
    new Dictionary<string, object> { 
      { "timestamp", new DateTime(2016, 1, 2, 12, 0, 0) },
      { "amount", 40900 }
    },
    new Dictionary<string, object> {
      { "timestamp", new DateTime(2016, 1, 3, 12, 0, 0) },
      { "amount", 16400 }
    }
  }, 
  "timestamp" // Delete By
);

`deleteBy` is an optional field by which to order the truncation of records once the maximum record count has been reached. By default the oldest records (by insertion time) will be removed.
```

## Development

To be confirmed
