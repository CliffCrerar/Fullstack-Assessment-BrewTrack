const
    [{ existsSync, readFileSync, writeFileSync }, { join }] = [require('fs'), require('path')],
    settingsFile = p('appsettings.json'),
    settingsDevFile = p('appsettings.Development.json'),
    settingsMergedFile = p('merged-appsettings.json'),
    appSettingsJson = readJsonFile(settingsFile),
    appSettingsDevJson = readJsonFile(settingsDevFile),
    newObj = Object.assign(appSettingsJson, {
        WeatherApiKey: appSettingsDevJson.WeatherApiKey, ConnectionStrings: appSettingsDevJson.ConnectionStrings
    });

function p(...relFile) {
    return join(__dirname, ...relFile)
}

function readJsonFile(filePath) {
    if (!existsSync(filePath)) {
        throw new Error('File does not exist')
    }
    return JSON.parse(readFileSync(filePath).toString());
}

function writeJsonToFile(obj, filePath) {
    const objToString = JSON.stringify(obj);
    writeFileSync(filePath, objToString, 'utf-8');
}

if (!existsSync(settingsDevFile)) {
    process.exit(0);
}

writeJsonToFile(newObj, settingsMergedFile);




