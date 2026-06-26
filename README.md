# توضیحات پروژه

این پروژه روی پیاده‌سازی یک سرویس **سبد خرید (Basket Service)** با استفاده از **Clean Architecture** و **CQRS** تمرکز دارد. هدف اصلی، طراحی یک ساختار تمیز، قابل توسعه و قابل نگهداری بود تا هر بخش از منطق پروژه در لایه‌ی مناسب خودش قرار بگیرد.

برای مدیریت **Command** و **Query** از **MediatR** استفاده شده و Eventهای داخلی پروژه نیز با همین ابزار مدیریت می‌شوند. همچنین **RabbitMQ** و **MassTransit** در پروژه راه‌اندازی و پیکربندی شده‌اند، اما در این تسک از آن‌ها برای انتشار Eventها استفاده نشده است.

---

# چالش‌های پروژه

بزرگ‌ترین چالش این پروژه خود کدنویسی نبود، بلکه تحلیل نیازمندی‌ها و رسیدن به یک معماری مناسب بود. مستند پروژه که به صورت فایل Word ارائه شده بود، ساختار منظم و شفافی نداشت و بعضی قسمت‌های آن برداشت‌های متفاوتی ایجاد می‌کرد. به همین دلیل بخش قابل توجهی از زمان صرف تحلیل نیازمندی‌ها، طراحی معماری و تصمیم‌گیری درباره ساختار پروژه شد.

---

# کارهای انجام شده

## معماری و طراحی

* طراحی ساختار پروژه بر اساس **Clean Architecture**
* پیاده‌سازی الگوی **CQRS** و جداسازی کامل Command و Query
* طراحی و پیاده‌سازی **Command Dispatcher** و **Query Dispatcher**
* طراحی و پیاده‌سازی **ICommand**، **IQuery**، **ICommandHandler** و **IQueryHandler**
* طراحی **DTO**های مجزا برای Commandها و Queryها و جلوگیری از بازگرداندن مستقیم Entityها
* مدیریت **Dependency Injection** با استفاده از Marker Interfaceها

## Domain-Driven Design

* طراحی موجودیت‌های دامنه **Basket** و **BasketItem**
* پیاده‌سازی قوانین دامنه (Domain Rules) داخل Entityها
* پیاده‌سازی **Domain Events** و مکانیزم ثبت و انتشار آن‌ها
* مدیریت انتشار Domain Eventها پس از ثبت تغییرات

## پیاده‌سازی قابلیت‌های سبد خرید

* دریافت یا ایجاد سبد خرید کاربر
* افزودن کالا به سبد خرید
* تغییر تعداد کالا
* حذف کالا از سبد خرید
* خالی کردن کامل سبد خرید
* پیاده‌سازی فرآیند **Expire Basket** و مدیریت وضعیت سبدهای منقضی

## اعتبارسنجی و مدیریت خطا

* پیاده‌سازی **ValidationBehavior** با MediatR Pipeline
* اعتبارسنجی Commandها با استفاده از **FluentValidation**
* طراحی و پیاده‌سازی **Domain Exception**
* مدیریت متمرکز خطاها
* طراحی و پیاده‌سازی **ServiceResult** و **ServiceResult<T>**
* استانداردسازی خروجی API با استفاده از **ToApiResult**

## Persistence

* پیاده‌سازی **Repository Pattern** و **Unit of Work**
* طراحی و پیاده‌سازی **IBasketRepository**
* پیاده‌سازی Repositoryها با **Entity Framework Core**
* پیاده‌سازی **Fluent API Configurations**
* ایجاد **Migration** و ساختار پایگاه داده
* استفاده از **AsNoTracking** در Queryها
* استفاده از **CancellationToken** در Repositoryها و Handlerها

## Pipeline و Transaction

* پیاده‌سازی **TransactionBehavior** برای مدیریت تراکنش‌ها
* مدیریت Commit و Rollback تراکنش‌ها
* Dispatch کردن Domain Eventها در پایان تراکنش

## Cache و Messaging

* راه‌اندازی و پیکربندی **Redis**
* پیاده‌سازی کش سبد خرید
* ذخیره، بازیابی و **Cache Invalidation** پس از تغییرات
* راه‌اندازی و پیکربندی **RabbitMQ**
* راه‌اندازی و پیکربندی **MassTransit**
* پیاده‌سازی **Background Service** برای اجرای دوره‌ای فرآیند منقضی‌سازی سبدها

## API

* پیاده‌سازی APIهای مربوط به سبد خرید
* استانداردسازی پاسخ‌های API
* استفاده از **Primary Constructors**
* استفاده از **Expression-bodied Members** در بخش‌های مناسب پروژه
* پیاده‌سازی عملیات **Asynchronous** در لایه‌های Application و Infrastructure

## Docker
 
* Dockerize کردن پروژه
* ایجاد **Dockerfile**
* ایجاد **docker-compose**
* راه‌اندازی **SQL Server**، **Redis**، **RabbitMQ** و **API** در Docker
* پیکربندی ارتباط بین سرویس‌های Docker و مدیریت Connection Stringها
