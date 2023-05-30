import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { NavItem } from 'reactstrap';

export function Home(props) {
  const displayName = props.name;

    return (
      <div>
        <h1 className="display-1">Welcome</h1>
        <p className="display-6">BrewTrack, is built with:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
          <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        </ul>
        <p className="display-6">Jump right in:</p>
        <NavItem tag={Link} to="/login">
        <button className="btn btn-primary btn-large">Get Started!</button>
        </NavItem>
      </div>
    );
}
