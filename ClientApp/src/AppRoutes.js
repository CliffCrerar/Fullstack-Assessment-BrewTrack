import { About, Breweries, Home, Identity, Register } from './components'

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
		path: '/breweries',
		element: <Breweries />
	},
	{
		path: '/about',
		element: <About/>
	}
];

export default AppRoutes;
