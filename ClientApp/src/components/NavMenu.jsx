import React, { useEffect, useRef, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Offcanvas, OffcanvasBody, OffcanvasHeader } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.scss';

export function NavMenu(props) {

	const displayName = props.name;
	const [toggleState, setState] = useState({ open: true });
	const [offCanvasState, setOffCanvasState] = useState(false);
	const toggleNavbar = () => {
		setState({ open: !toggleState.open });
	}

	const elementRef = useRef();

	const toggleOffCanvas = () => setOffCanvasState(!offCanvasState)

	function NavLinks() {
		return (
			<ul className="navbar-nav flex-grow">
				<NavItem>
					<NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
				</NavItem>
				{/* <NavItem>
					<NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
				</NavItem>
				<NavItem>
					<NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
				</NavItem> */}
			</ul>
		)
	}

	return (
		<>
			<header>
				<Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
					<NavbarBrand tag={Link} to="/">{displayName}</NavbarBrand>
					<NavbarToggler onClick={toggleOffCanvas} className="mr-2 btn" ></NavbarToggler>
					<Collapse isOpen={toggleState.open}>
						<NavLinks />
					</Collapse>
				</Navbar>
			</header >
			<Offcanvas
				direction="end"
				isOpen={offCanvasState}
			>
				<OffcanvasHeader>
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
