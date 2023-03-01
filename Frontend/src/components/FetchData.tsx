import React, {useEffect, useState} from 'react';

export let FetchData = () => {
  const [state, setState] = useState({
    forecasts: [],
    loading: true
  })
  
  useEffect(() => {
    (async () => {
      const response = await fetch("api/weatherforecast");
      const data = await response.json();
      setState({forecasts: data, loading: false});
    })()
  }, [])

  let contents = state.loading
      ? <p><em>Loading...</em></p>
      : <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
        </thead>
        <tbody>
        {state.forecasts.map((forecast:any) =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
        )}
        </tbody>
      </table>;

  return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
  );
}
