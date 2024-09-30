import React, { Component } from 'react';

export class Weather extends Component {
  static displayName = Weather.name;

  constructor(props) {
    super(props);
    this.state = { 
      show: false, 
      forecasts: [], 
      city: '', 
      country: '', 
      loading: true, 
      searchText: ''
    };
  }

  componentDidMount() {
    
  }

  handleSearchChange = (event) => {
    this.setState({ searchText: event.target.value });
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Weather.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        <input type='text' 
          value={this.state.searchText}
          placeholder='Enter search text'
          onChange={this.handleSearchChange}/>
        <button onClick={this.populateWeatherData}>Search</button>
        <h3><span>{this.state.city}</span> - <span>{this.state.country}</span></h3>
        { this.state.show ? contents : null }
      </div>
    );
  }

  populateWeatherData = async () => {
    if (this.state.searchText.trim() == '') {
      alert("Enter search text");
      return;
    }
    this.setState({ 
      loading: true,
      show: true,
      city: '',
      country: ''
    });
    const response = await fetch(`weatherforecast/search?text=${this.state.searchText}`);
    if (!response.ok) {
      this.setState({
        forecasts: [],
        city: '',
        loading: false,
        show: false
      });
      alert("City not found")
      return;
    }
    const data = await response.json();
    this.setState({ 
      forecasts: data.forecasts,
      city: data.city,
      country: data.country,
      loading: false
    });
  }
}
