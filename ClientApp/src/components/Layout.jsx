import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export function Layout(props) {
	const displayName = props.name;

	return (
		<div>
			<NavMenu name={displayName} />
			<Container>
				{props.children}
			</Container>
		</div>
	);
}
