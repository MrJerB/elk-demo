# ELK Intro

## ES Basic Terminology
Node - Instance of ElasticSearch
Cluster - Set of one or more nodes (distribution)
Document - JSON-based, has a unique ID.. contains fields
Index - Used to be... A group of different types. 
Shard - dividing data from an index across multiple nodes (so capacity of an index can exceed the storage space on just one node)
Replicas - High availability! A shard can either be primary or a replica. Each document in an index belongs to one primary shard, replica is a copy.. redundancy. You control the number of primary shards per index when creating that index. The number of replicas can be changed whenever.

### Type Mappings Deprecated/Removed!

Previously: Index = Database, Type = Table.. But this was a bad analogy since fields with the same name in different types had the same backing field in Lucene index (the search engine behind ES)

Mapping types are removed in the recent versions of ElasticSearch. The type field used to be used to combine with the document `_id` to create a unique id `_uid`. Documents with different types could exist in the same index. Now the id fields are alias of each other.

# Kibana
* Querying via Lucene or KQL


# Local Setup (WSL2 + Docker Desktop) incase of ES startup issue
```
wsl -d docker-desktop
sysctl -w vm.max_map_count=262144
```

# Logstash filters!
https://www.elastic.co/guide/en/logstash/current/filter-plugins.html