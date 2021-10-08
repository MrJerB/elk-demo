Push-Location $PSScriptRoot

docker run --rm -it --network host --name ls -v ${PWD}/pipeline:/usr/share/logstash/pipeline -v ${PWD}/data/:/usr/data/ docker.elastic.co/logstash/logstash-oss:8.0.0-alpha2

Pop-Location