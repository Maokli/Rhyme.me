
    import React, { Component } from 'react';
    import Speech from './components/Speech/speech';
    import './App.css';

    class App extends Component {
      state = {
        words : []
      }

      
      render() {
        return (
          <div>
            <Speech></Speech>
          </div>
        );
      }
    }

    export default App;
