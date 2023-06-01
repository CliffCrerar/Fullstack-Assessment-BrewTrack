import React, { useEffect, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Offcanvas, OffcanvasBody, OffcanvasHeader } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.scss';

export function NavMenu(props) {

	const displayName = props.name;
	const [toggleState, setState] = useState({ open: false });
	const toggleNavbar = () => {
		setState({ open: !toggleState.open });
	}

	function noRefCheck(some) {
		console.log("🚀 ~ file: NavMenu.js:15 ~ noRefCheck ~ some:", some)
	}

	function NavLinks() {
		return (
			<ul className="navbar-nav flex-grow">
				<NavItem>
					<NavLink tag={Link} onClick={toggleNavbar} className="text-dark" to="/">Home</NavLink>
				</NavItem>
				<NavItem>
					<NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
				</NavItem>
				<NavItem>
					<NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
				</NavItem>
			</ul>
		)
	}

	return (
		<>
			<header>
				<Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
					<NavbarBrand tag={Link} to="/">{displayName}</NavbarBrand>
					<NavbarToggler onClick={toggleNavbar} className="mr-2 btn" ></NavbarToggler>
					<Collapse>
						<NavLinks />
					</Collapse>
				</Navbar>
			</header>
			<Offcanvas
				direction="end"
				isOpen={toggleState.open}
				toggle={toggleNavbar}
			>
				<OffcanvasHeader toggle={toggleNavbar}>
					Offcanvas
				</OffcanvasHeader>
				<OffcanvasBody>
					<strong>
						This is the Offcanvas body.
					</strong>
					<NavLinks />
				</OffcanvasBody>
			</Offcanvas>
		</>
	);
}
