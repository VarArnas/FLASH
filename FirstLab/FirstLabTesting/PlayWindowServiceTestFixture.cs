using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.services;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabTesting;

public class PlayWindowServiceTestFixture
{
    public IFactoryContainer FactoryContainer { get; private set; }
    public PlayWindowService PlayWindowService { get; private set; }

    public PlayWindowServiceTestFixture()
    {
    }

    public void ResetMocks()
    {
        var mockFactoryContainer = new Mock<IFactoryContainer>();
        FactoryContainer = mockFactoryContainer.Object;
        PlayWindowService = new PlayWindowService(FactoryContainer);
    }
}
