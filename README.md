# UnlimitedMongoID

Tarkov MongoID is a 24 byte length ID

Consisting of TimeStamp (0-8) and Counter (8-24)
[MongoDB ObjectId generator](https://observablehq.com/@hugodf/mongodb-objectid-generator)

It was previously used for specific items, like bullet

This brings limitations

When Modder used any non-MongoID, it will cause error

Now in 3.10.0 (0.15), BSG applied it to almost all ID

This forced modders who had not used MongoID to change almost all ID in order to be compatible with new version

Therefore this mod remove MongoID format restrictions

Modder can use any ID like before