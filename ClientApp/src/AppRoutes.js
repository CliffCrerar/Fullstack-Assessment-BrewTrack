import { Breweries, Counter, FetchData, Home, Identity, Register } from './components'

const AppRoutes = [
	{
		index: true,
		element: <Home />
	},
	{
		path: '/login',
		element: <Identity />
	},
	{
		path: '/register',
		element: <Register />
	},
	{
		path: '/counter',
		element: <Counter />
	},
	{
		path: '/fetch-data',
		element: <FetchData />
	},
	{
		path: '/breweries',
		element: <Breweries />
	}
];

export default AppRoutes;
