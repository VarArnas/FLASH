﻿using FirstLab.src.data;
using FirstLab.src.factories;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.models.DTOs;
using FirstLab.src.utilities;
using FirstLab.XAML;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FirstLab.src.services;

public class FlashcardOptionsService : IFlashcardOptionsService
{
    IFactoryContainer _factoryContainer;

    IFlashcardSetMapper _ifFlashcardSetMapper;

    public FlashcardOptionsService(IFactoryContainer factoryContainer, IFlashcardSetMapper ifFlashcardSetMapper)
    {
        _factoryContainer = factoryContainer;
        _ifFlashcardSetMapper = ifFlashcardSetMapper;
    }

    public async Task RemoveFlashcardSet(FlashcardSet selectedSet, ObservableCollection<FlashcardSet> flashcardSets)
    {
        await DatabaseRepository.RemoveFlashcardSetAsync(selectedSet.FlashcardSetName);
        flashcardSets.Remove(selectedSet);
    }

    public void InitializeDatabase(IServiceProvider serviceProvider)
    {
        DatabaseRepository.serviceProvider = serviceProvider;
    }

    public async Task InitializeFlashcardSets(ObservableCollection<FlashcardSet>? flashcardSets)
    {
        ObservableCollection<FlashcardSetDTO> flashcardSetsDTOs = await DatabaseRepository.GetAllFlashcardSetsAsync();

        foreach(var dto in flashcardSetsDTOs)
        {
            FlashcardSet set = _ifFlashcardSetMapper.TransformDTOtoFlashcardSet(dto);
            flashcardSets!.Add(set);
        }
    }

    public ObservableCollection<String> CalculateFlashcardSetDifficulties(ObservableCollection<FlashcardSet> flashcardSets)
    {
        ObservableCollection<String> difficulties = new ObservableCollection<String>();
        foreach(var flashcardSet in flashcardSets)
        {
            difficulties.Add(CalculateDifficultyOfFlashcardSet(flashcardSet));
        }

        return difficulties;
    }
    
    public string CalculateDifficultyOfFlashcardSet(FlashcardSet set)
    {
        string difficulty = "Medium";

        if (set.Flashcards != null)
        {
            int score = 0;
            int numberOfFlashcards = set.Flashcards.Count;

            foreach (Flashcard flashcard in set.Flashcards)
            {
                switch (flashcard.FlashcardColor)
                {
                    case "red":
                        score += 1;
                        break;

                    case "green":
                        score += 2;
                        break;

                    case "yellow":
                        score += 3;
                        break;

                    case "blue":
                        score += 4;
                        break;

                    case "orange":
                        score += 5;
                        break;
                }
            }

            if (numberOfFlashcards > 0)
            {
                int difficultyNumber = (int)Math.Round((decimal)score / numberOfFlashcards);
                switch (difficultyNumber)
                {
                    case 1:
                        difficulty = "Very easy";
                        break;

                    case 2:
                        difficulty = "Easy";
                        break;

                    case 3:
                        difficulty = "Medium";
                        break;

                    case 4:
                        difficulty = "Hard";
                        break;

                    case 5:
                        difficulty = "Very Hard";
                        break;
                }
            }
        }

        return difficulty;
    }

    public void GoToFlashcardCustomization(FlashcardSet? flashcardSet = null)
    {
        FlashcardCustomization flashcardCustomization = _factoryContainer.CreateWindow<FlashcardCustomization>(flashcardSet);
        ViewsUtils.ChangeWindow("Customization", flashcardCustomization);
    }

    public void LaunchPlayWindow(FlashcardSet flashcardSet)
    {
        PlayWindow playWindowReference = _factoryContainer.CreateWindow<PlayWindow>(flashcardSet);
        ViewsUtils.menuWindowReference!.Hide();
        playWindowReference.Show();
    }
}
