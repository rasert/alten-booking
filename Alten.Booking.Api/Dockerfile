#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Alten.Booking.Api/Alten.Booking.Api.csproj", "Alten.Booking.Api/"]
COPY ["Alten.Booking.Application/Alten.Booking.Application.csproj", "Alten.Booking.Application/"]
COPY ["Alten.Booking.Domain/Alten.Booking.Domain.csproj", "Alten.Booking.Domain/"]
COPY ["Alten.Booking.Infrastructure/Alten.Booking.Infrastructure.csproj", "Alten.Booking.Infrastructure/"]
RUN dotnet restore "Alten.Booking.Api/Alten.Booking.Api.csproj"
COPY . .
WORKDIR "/src/Alten.Booking.Api"
RUN dotnet build "Alten.Booking.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Alten.Booking.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Alten.Booking.Api.dll"]