import React, { useEffect, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.scss';

export function NavMenu(props) {

  const displayName = props.name;
  const [state, setState] = useState({ open: true });
  function toggleNavbar() {

    const state = { collapsed: !state.collapsed }
    setState(state);
  }

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
        <NavbarBrand tag={Link} to="/">{displayName}</NavbarBrand>
        <NavbarToggler onClick={() => toggleNavbar} className="mr-2 btn" >Toggler</NavbarToggler>
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={state.open} navbar>
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
            </NavItem>
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}
