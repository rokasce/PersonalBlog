FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY . ./

RUN dotnet publish ./PersonalBlog.sln -c $BUILDCONFIG -o /app/out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "API.dll"]