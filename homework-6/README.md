# Ozon Route 256 — Kafka Homework

### Docker
* Если у вас всё ещё нет docker — его нужно поставить.
* Запустить docker контейнеры (БД): `docker compose up -d`
* Остановить docker контейнеры (БД): `docker compose down`
* Остановить и почистить docker от данных: `docker compose down -v`
* Docker поломался: `docker system prune`
* Не подключается к kafka? Добавьте `127.0.0.1 kafka` к вашему hosts-файлу

### Kafka
* Заводите Kafka через docker. Пример docker-compose-файла с Kafka находится в репозитории воркшопа этой недели: https://gitlab.ozon.dev/cs/classroom-6/Week-6/workshop/-/blob/master/docker-compose.yml
* Offset Explorer (ранее называлось Kafka Tool): https://www.kafkatool.com/ — позволяет читать и писать в Apache Kafka через простой UI
* Серьезные девчата и пацаны качают Kafka (https://kafka.apache.org/downloads) и используют sh/bat-файлы оттуда, чтобы читать/писать/манипулировать Кафкой. Допускается быть несерьезными и использовать Offset Explorer.

### Домашнее задание
Помните соседнюю команду, которой нужны были данные по расчётам всех товаров и мы сделали им консольное приложение, ведь это нужно было всего один раз? Команда пришла вновь и говорит, что теперь это нужно делать постоянно, и консолечка им не подходит. Никогда такого не было, и вот опять!© Мы ох как удивились, подумали и предложили схему:
* Создаем топик Kafka `good_price_calc_requests` с входящими параметрами, аналогичными тем, что есть в последней версии API. Дополнительно ребята добавляют ещё поле с идентификатором товара во Всем Известной Кроме Нас Системе Товаров Озона. Формат сообщений — JSON. Наполнение топика остается за соседней командой. Наша задача — читать этот топик и делать расчёты.
* Создаём топик Kafka `good_price_calc`, в который мы будем писать результаты расчёта вместе с идентификатором товара, чтобы связать результаты с запросом. Формат сообщений — также JSON. Чтение топика и применение этих данных для своих нужд, слава байтам, сможет сделать соседняя команда.
* Создаём топик `good_price_calc_requests_dlq`, в который мы будем писать невалидные сообщения, если такие будут. Разбор этого топика на стадии MVP не предполагается.

### Бонусные алмазики
* Так как соседняя команда перегружена Очень Важными Задачами Лично От Директора, то она наградит нас одним алмазиком, если мы напишем сервис, который будет наполнять топик `good_price_calc_requests` актуальными товарами. Ребята очень заняты, поэтому их устроит даже консольное приложение. Они не сказали, где брать список актуальных товаров и ОВХ. Вероятно, подойдет Мистер Псевдослучайный Рандом. Было бы здорово, если б можно было управлять через входные параметры количеством генерируемых товаров.
* Мы беспокоимся, поэтому дополнительно хотим записывать к себе в БД аномальные результаты расчётов товаров. Например, стоимость выше 10000 рублей (подберите цифру эмпирически, когда сделаете основную часть домашней работы). Мы не хотим тормозить расчёт, поэтому применим схему асинхронного взаимодействия — создадим ещё один hosted-сервис, который читает наши же результаты расчёта из топика `good_price_calc` и сохраняет в БД такие аномалии. За такую работу тимлид одобрительно похлопает по плечу, продакт менеджер кивнёт и пробубнит что-то про бизнесовые метрики, а у вас появится алмазик.

### Скучные, неинтересные, зевотные ответы на вопросы, которые никто не задавал
Q: Что брать за основу проекта?  
A: За основу берется домашнее задание предыдущей недели, либо проект из текущего репозитория — на ваше усмотрение. Не забудьте предупредить тьютора, как вы решили это сделать.

Q: Как сделать задачу?  
A: Нужно создать [Background Hosted Service](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio), который будет слушать входящий топик и отправлять сообщения в исходящий. Под бонусную задачу нужно создать отдельный бекграунд-сервис.

Q: Где разместить бекграунд сервисы?  
A: Давайте создадим отдельную assembly `Route256.Week6.Homework.PriceCalculator.BackgroundServices`.

Q: Какой формат сообщения?  
A: JSON. Одно сообщение — один товар.
```json
{
    "good_id": 0,
    "height": 0.0,
    "length": 0.0,
    "width": 0.0,
    "weight": 0.0
}
```

Q: А выходной?  
A: JSON. 
```json
{
    "good_id": 0,
    "price": 0.0
}
```

Q: Что такое невалидные сообщения?  
A: Которые как валидные, только наоборот. Те, которые не удалось сериализовать из json в нужный контракт. В проде, к сожалению, такое может случиться. И если такое сообщение не обработать особенным образом (отправив в dlq), то оно застопорит разбор партиции. А еще такими сообщениями можно считать те, которые удалось сериализовать, но мы не можем посчитать такой товар — например, отрицательный вес (антигравитация в этой мультивселенной ещё не открыта) и подобные вещи.