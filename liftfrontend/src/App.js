import React, { Component } from 'react';
import Lifts from './components/lifts';
import CallButton from './components/callbutton';
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="App">
        <Lifts />
        <CallButton />
      </div>
    );
  }
}

export default App;