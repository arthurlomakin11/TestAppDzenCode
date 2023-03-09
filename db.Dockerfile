FROM postgres:13.10
ADD dump.sql /docker-entrypoint-initdb.d/