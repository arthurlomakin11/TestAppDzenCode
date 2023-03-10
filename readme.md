# TestAppDzenCode

SPA-приложение на C# и React.
БД - PostreSQL

## Установка

Для Linux:

```
curl -fsSL https://get.docker.com -o get-docker.sh
```
```
sh get-docker.sh
```
```
 sudo curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
```
```
sudo chmod +x /usr/local/bin/docker-compose
```
```
git clone https://github.com/arthurlomakin11/TestAppDzenCode.git
```
```
cd TestAppDzenCode
```

```
sudo docker compose up -d
```

Приложение будет доступно с портом 4000

Можно подключится к PostgreSQL: порт 5432

Database = TestAppDzenCodeDB

Username = user

Password = user

## Инстанс на AWS
[Инстанс](http://ec2-16-171-0-168.eu-north-1.compute.amazonaws.com:4000/) развернут на Ubuntu.

Также, можно подключится к БД по ec2-16-171-0-168.eu-north-1.compute.amazonaws.com:5432

## Схема БД

![alt text](https://github.com/arthurlomakin11/TestAppDzenCode/raw/master/schema.png)