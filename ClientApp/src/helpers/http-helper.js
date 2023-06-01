
const headers = new Headers();

export async function getFromApi(apiPath) {
	return await fetch(apiPath);
}

export async function postToApi(apiPath, body) {
	headers.append('Content-Type', 'application/json');
	const
		options = {
			headers,
			method: 'POST',
			cache: 'default',
			body: JSON.stringify(body)
		},
		request = new Request(apiPath)
	return await fetch(request, options);
}

