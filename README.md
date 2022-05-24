# SWG_match3_example
SWG match3 test work

Тестовое задание для компании SWG Games. Описание в файле: "Тестовое задание на вакансию «Разработчик Unity».pdf"

Хотелось показать умение строить архитектуру проекта, знание DI (выбран Zenject), а также плашинов Dotween, RSG Promises (https://github.com/Real-Serious-Games/C-Sharp-Promise/blob/master/README.md#getting-the-dll).

В процессе разработки примененены паттерны StaeMachine, Strategy, Simple Factory.

Для переиспользования фишек на сцене используется пул объектов. 

Благодаря использованию менеджера асинхронных операций RSG Promises - возможно использование последовательностей и анимаций любой сложности, с возможностью контроля времени запуска и окончания.