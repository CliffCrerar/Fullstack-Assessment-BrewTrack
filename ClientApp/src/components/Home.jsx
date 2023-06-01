import { Link, useNavigate } from 'react-router-dom';
import { NavItem } from 'reactstrap';
import { useEffect } from 'react';
import _ from "lodash";

/**
 * @description The home component is where a user lands for the very first time visiting, the home component will record a users action in the local storage and subsequently a user using the same device will not be presented with this screen again.
 */
export function Home(props) {
	const displayName = props.name;

	// component text
	const componentText = {
		welcome: 'Welcome',
		welcomeSubTitle: 'BrewTrack, is built with:',
		linkOne: { a: 'ASP.NET Core', b: ' and ', c: 'C#', d: 'for cross-platform server-side code' },
		linkTwo: { a: 'React', b: ' for client-side code' },
		linkThree: { a: 'Bootstrap', b: ' for layout and styling' },
		paragraph: 'Jump right in:',
		getStartedBtn: 'Get Started!'
	}

	const hasStarted = localStorage.getItem("hasStarted"); // retrieve item from local storage

	const navigate = useNavigate(); // function used for navigation

	// use effect hook, to see if user has previously clicked get started,
	// but only does so once rendering is complete
	// if true nav to identification
	useEffect(() => {
		if (Boolean(hasStarted)) {
			navigate('/login')
		}
	})

	return (
		<div>
			<h1 className="display-1">{componentText.welcome}</h1>
			<p className="display-6">{componentText.welcomeSubTitle}</p>
			<ul>
				<li>
					<a href='https://get.asp.net/'>{componentText.linkOne.a}</a>{componentText.linkOne.b}
					<a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>{componentText.linkOne.c}</a>{componentText.linkOne.d}
				</li>
				<li>
					<a href='https://facebook.github.io/react/'>{componentText.linkTwo.a}</a>{componentText.linkTwo.b}
				</li>
				<li>
					<a href='http://getbootstrap.com/'>{componentText.linkThree.a}</a>{componentText.linkThree.b}
				</li>
			</ul>
			<p className="display-6">{componentText.paragraph}</p>
			<NavItem tag={Link} to="/login">
				<button onClick={() => localStorage.setItem('hasStarted', true)} className="btn btn-primary btn-large">{componentText.getStartedBtn}</button>
			</NavItem>
		</div>
	);
}
