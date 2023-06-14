import { GiBeerHorn } from 'react-icons/gi';
export function About() {
    return <div>
        <div className='d-flex justify-content-between align-items-center'>
            <h1 class="display-1">About</h1>
            <GiBeerHorn style={{ fontSize: '5em' }} />
        </div>
        <hr />
        <p className="lead">This application was constructed as a full stack project assessment. For this purpose the following components and technologies were selected and integrated into the application.</p>
        <h2>Front End</h2>
        <hr></hr>
        <div className="d-grid" style={{ gridTemplateColumns: "auto auto auto auto auto", marginBottom: '50px' }}>
            <div>
                <img className="img-fluid" src="/img/react.png" alt="React" />
            </div>
            <div>
                <img className="img-fluid" src="/img/recharts.png" alt="recharts" />
            </div>
            <div>
                <img className="img-fluid" src="/img/lodash.png" alt="lodash" />
            </div>
            <div>
                <img className="img-fluid" src="/img/react-icons.svg" alt="React Icons" />
            </div>
            <div>
                <img className="img-fluid" src="/img/bootstrap.png" alt="Bootstrap" />
            </div>
        </div>
        <h2>Back End</h2>
        <hr></hr>
        
            <div className="d-grid" style={{ gridTemplateColumns: "auto auto auto auto", marginBottom: '50px' }}>
                <div>
                    <img className="img-fluid" src="/img/asp.net.svg" alt="ASP.Net" />
                </div>
                <div>
                    <img className="img-fluid" src="/img/efcore.svg" alt="Entity Framework" />
                </div>
                <div>
                    <img className="img-fluid" src="/img/redis.svg" alt="lodash" />
                </div>
                <div>
                    <img className="img-fluid" src="/img/mysql.png" alt="React Icons" />
                </div>

            </div>
        
        <h2>External API</h2>

        <hr></hr>
        <div className="d-grid" style={{ gridTemplateColumns: "auto auto auto auto", marginBottom: '50px' }}>
                <div>
                    <img className="img-fluid" src="/img/stormglass.png" alt="Stormglass.io" />
                </div>
                <div>
                    <img className="img-fluid" src="/img/obdb-og.png" alt="Open Breweries Database" />
                </div>
            </div>

        <h3>Author</h3>
        <hr/>

        <div class="badge-base LI-profile-badge" data-locale="en_US" data-size="medium" data-theme="light" data-type="HORIZONTAL" data-vanity="cliff-crerar" data-version="v1"><a class="badge-base__link LI-simple-link" href="https://za.linkedin.com/in/cliff-crerar?trk=profile-badge">Clifford Crerar</a></div>
              

        <div style={{height: "250px"}}></div>
    </div>
}