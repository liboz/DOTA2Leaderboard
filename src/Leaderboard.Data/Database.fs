namespace Leaderboard.Data

open MongoDB.Bson;
open MongoDB.Driver;

module Database =
    let client = lazy(new MongoClient());
    let database = lazy(client.Value.GetDatabase("test"))
    let db = database.Value

    let insertData data region = 
        let document = BsonDocument.Parse(data);

        let collection = db.GetCollection<BsonDocument>(region)
        Async.AwaitTask (collection.InsertOneAsync(document))

    let getData region date = 
        let collection = db.GetCollection<BsonDocument>(region)
        let filter = FilterDefinition<BsonDocument>.op_Implicit("{}")  
        let mutable count = 0
        use cursor = collection.FindSync(filter)
        while (cursor.MoveNext()) do
            let batch = cursor.Current
            for document in batch do
                printfn "%A" document
                count <- count + 1
        printfn "%A" count
