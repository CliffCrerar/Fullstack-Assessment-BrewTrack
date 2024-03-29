import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.scss';

export default function App() {
	const displayName = "BrewTrack";
	return (
		<Layout name={displayName}>
			<Routes>
				{AppRoutes.map((route, index) => {
					const { element, ...rest } = route;
					return <Route key={'route-' + index} {...rest} element={element} />;
				})}
			</Routes>
		</Layout>
	);
}
