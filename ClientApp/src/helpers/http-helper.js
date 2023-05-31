
const headers = new Headers();

export async function getFromApi(apiPath) {
    return await fetch(apiPath);
}

export async function postToApi(apiPath, body) {
    headers.append('Content-Type','application/json');
    const options = {
        headers,
        method: 'GET',
        cache: 'default',
        body: JSON.stringify(body)
    },
    request = new Request(apiPath),
    response = await fetch(request,options);
    return response.json();
}