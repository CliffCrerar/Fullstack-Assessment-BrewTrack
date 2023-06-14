import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './';

export function Layout(props) {
	const displayName = props.name;
	const styleClasses = {flex: 1, display: 'flex', flexDirection: "column"}
	return (
		<>
			<NavMenu name={displayName} />
			<Container style={styleClasses}>
				{props.children}
			</Container>
		</>
	);
}
