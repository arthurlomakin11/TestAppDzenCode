import React, {useState} from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export let NavMenu = () => {
  const [toggleNavbar, setToggleNavbar] = useState(false);
  
  function toggleNavbarChange() {
    setToggleNavbar(!toggleNavbar)
  }
  
  return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">TestAppDzenCode</NavbarBrand>
          <NavbarToggler onClick={toggleNavbarChange} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={toggleNavbar} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Navbar>
      </header>
  );
}
